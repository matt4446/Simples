<script lang="ts">
  import { onMount } from "svelte";
  import * as Card from "$lib/components/ui/card";
  import Button from "$lib/components/ui/button/button.svelte";
  import { chatService } from "../../services/chatService";
  import { finalize, pipe, tap, catchError, of } from "rxjs";
  import { marked } from 'marked';
  import MessageList from "./MessageList.svelte";
  import ChatInput from "./ChatInput.svelte";
  import type { Message } from "../../types";

  let messages: Message[] = [];
  let isLoading = false;

  // Load messages from localStorage on mount
  onMount(() => {
    const stored = localStorage.getItem('chatMessages');
    if (stored) {
      messages = JSON.parse(stored);
    }
  });

  // Save messages to localStorage whenever they change
  $: {
    if (messages.length > 0) {
      // Keep only the last 5 messages
      const lastFiveMessages = messages.slice(-5);
      localStorage.setItem('chatMessages', JSON.stringify(lastFiveMessages));
    }
  }

  function handleKeydown(event: KeyboardEvent) {
    console.log("handle keydown");
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      const target = event.target as HTMLTextAreaElement;
      const text = target.value.trim();
      if (text) {
        handleSendMessage(text);
        target.value = '';
      }
    }
  }

  function handleSendMessage(text: string) {
    console.log("handleSendMessage wrapper called with text:", text);
    if (!text?.trim()) {
      console.log("Message is empty, not sending");
      return;
    }
    sendMessage(text);
  }

  function processMarkdown(text: string): string {
    return marked.parse(text) as string;
  }

  async function sendMessage(text: string) {
    console.log("send message", text);
    if (!text.trim() || isLoading) {
      console.log("Message is empty or already loading, not sending", text, isLoading);
      return;
    }

    isLoading = true;
    const userTimestamp = new Date().toLocaleString();
    messages = [
      ...messages,
      {
        text: text,
        sender: "user",
        timestamp: userTimestamp,
        isLoading: false,
      },
    ];

    let botMessage: Message = {
      text: "",
      sender: "bot",
      timestamp: null,
      isLoading: true,
    };

    // Add the loading message immediately
    messages = [...messages, botMessage];

    chatService
      .ask(text)
      .pipe(
        tap((response) => {
          console.log(response);
        }),
        tap((apiResponse) => {
          botMessage.text = apiResponse.items[0].text;
          botMessage.error = false;
        }),
        catchError((error) => {
          console.error('Chat error:', error);
          botMessage.text = "Sorry, I encountered an error processing your request.";
          botMessage.error = true;
          return of(null);
        }),
        finalize(() => {
          botMessage.isLoading = false;
          botMessage.timestamp = new Date().toLocaleString();
          isLoading = false;
          // Update the bot message in place
          messages = messages.map(m => 
            m === botMessage ? { ...botMessage } : m
          );
        })
      )
      .subscribe();
  }

  function clearHistory() {
    messages = [];
    localStorage.removeItem('chatMessages');
  }
</script>

<Card.Root class="h-full flex flex-col">
  <Card.Header class="flex justify-between items-center">
    <h2 class="text-xl font-semibold">Chat Assistant</h2>
    <Button variant="outline" size="sm" onclick={clearHistory}>Clear History</Button>
  </Card.Header>
  <Card.Content class="flex-1 overflow-y-auto p-6 space-y-4 max-h-[600px]">
    <MessageList {messages} {processMarkdown} />
  </Card.Content>
  <Card.Footer class="p-4 border-t">
    <ChatInput 
      {isLoading} 
      {handleKeydown} 
      sendMessage={sendMessage} 
    />
  </Card.Footer>
</Card.Root>

<style>
  :global(.chat-message p) {
    margin-bottom: 0.5em;
  }
  :global(.chat-message p:last-child) {
    margin-bottom: 0;
  }
  :global(.chat-message pre) {
    background: rgba(0, 0, 0, 0.1);
    padding: 0.5em;
    border-radius: 4px;
    overflow-x: auto;
  }
  :global(.chat-message code) {
    font-family: monospace;
    background: rgba(0, 0, 0, 0.1);
    padding: 0.2em 0.4em;
    border-radius: 3px;
  }
</style>
