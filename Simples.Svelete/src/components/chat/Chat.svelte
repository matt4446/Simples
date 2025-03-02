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
  let messageContainer: HTMLDivElement | null = null;

  function scrollToBottom() {
    if (messageContainer) {
      messageContainer.scrollTop = messageContainer.scrollHeight;
    }
  }

  // Load messages from localStorage on mount
  onMount(() => {
    const stored = localStorage.getItem('chatMessages');
    if (stored) {
      messages = JSON.parse(stored);
    }
    scrollToBottom();
  });

  // Scroll to bottom whenever messages change
  $: {
    if (messages.length > 0) {
      // Keep only the last 5 messages
      const lastFiveMessages = messages.slice(-5);
      localStorage.setItem('chatMessages', JSON.stringify(lastFiveMessages));
      // Add a small delay to ensure the DOM has updated
      setTimeout(scrollToBottom, 0);
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
        author: "user"
      },
    ];

    chatService
      .ask(text)
      .pipe(
        tap((response) => {
          console.log(response);
        }),
        tap((apiResponses) => {
          apiResponses.forEach(response => {
            response.items.forEach(item => {
              if (item.text?.trim()) {
                let newBotMessage: Message = {
                  text: item.text,
                  sender: "bot",
                  timestamp: new Date().toLocaleString(),
                  isLoading: false,
                  error: false,
                  author: response.authorName
                };
                messages = [...messages, newBotMessage];
              }
            });
          });
        }),
        catchError((error) => {
          console.error('Chat error:', error);
          let errorMessage: Message = {
            text: "Sorry, I encountered an error processing your request.",
            sender: "bot",
            timestamp: new Date().toLocaleString(),
            isLoading: false,
            error: true,
            author: "assistant"
          };
          messages = [...messages, errorMessage];
          return of(null);
        }),
        finalize(() => {
          isLoading = false;
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
  <div bind:this={messageContainer} class="flex-1 overflow-y-auto p-6 space-y-4 max-h-[600px]">
    <MessageList {messages} {processMarkdown} />
  </div>
  <Card.Footer class="p-4 border-t">
    <ChatInput 
      {isLoading} 
      {handleKeydown} 
      {sendMessage}
      {messages}
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
