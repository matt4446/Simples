<script lang="ts">
  import {
    Icon,
    ArrowPathRoundedSquare,
    PaperAirplane,
  } from "svelte-hero-icons";
  import { onMount } from "svelte";
  import * as Card from "$lib/components/ui/card";
  import Button from "$lib/components/ui/button/button.svelte";
  import Textarea from "$lib/components/ui/textarea/textarea.svelte";
  import { chatService } from "../../services/chatService";
  import { finalize, pipe, tap } from "rxjs";

  interface Message {
    text: string;
    sender: "user" | "bot";
    timestamp: string | null;
    isLoading: boolean;
  }

  let messages: Message[] = [];
  let newMessage = "";
  let isLoading = false;

  async function sendMessage() {

    if (!newMessage.trim()) {
      return;
    }

    isLoading = true;
    const userTimestamp = new Date().toLocaleString();
    messages = [
      ...messages,
      {
        text: newMessage,
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

    chatService
      .ask(newMessage)
      .pipe(
        tap((response) => {
          console.log(response);
        }),
        tap((updatedResponse) => {
          botMessage.text = updatedResponse.choices[0].contents;
          botMessage.isLoading = false;
        }),
        finalize(() => {
          botMessage.isLoading = false;
          botMessage.timestamp = new Date().toLocaleString();

          isLoading = false;
        })
      )
      .subscribe();

      isLoading = true;
      newMessage = "";
  }
</script>

<Card.Root>
  <Card.Header>
    <h2>Chat</h2>
  </Card.Header>
  <Card.Content class="flex-1 overflow-y-auto p-6">
    {#if messages.length === 0}
      <p class="text-gray-500">No messages yet. Start the conversation!</p>
    {/if}

    {#each messages as message}
      <div
        class="mb-2 p-2 rounded-lg {message.sender === 'user'
          ? 'bg-gray-700 self-end'
          : 'bg-slate-700'}"
      >
        {message.text}
        <small class="text-gray-500">{message.timestamp}</small>
      </div>
    {/each}
  </Card.Content>
  <Card.Footer>
    <div class="relative w-full max-w-full justify-self-center flex gap-4">
      <Textarea
        bind:value={newMessage}
        placeholder="Type a message..."
        class="block resize-none rounded-md border-0 p-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:outline-none sm:text-sm/6 [field-sizing:content] dark:text-gray-100 dark:bg-gray-800 dark:ring-gray-600"
      />

      <Button onclick={() => sendMessage()} disabled={isLoading}>
        {#if isLoading}
          <Icon src={ArrowPathRoundedSquare} class="animate-spin" />
        {:else}
          <Icon src={PaperAirplane} />
          Send
        {/if}
      </Button>
    </div>
  </Card.Footer>
</Card.Root>
