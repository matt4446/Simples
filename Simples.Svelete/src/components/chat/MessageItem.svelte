<script lang="ts">
  import type { Message } from "../../types";
  import { Icon, ArrowPathRoundedSquare, ExclamationCircle } from "svelte-hero-icons";

  export let message: Message;
  export let processMarkdown: (text: string) => string;
</script>

<div class="flex {message.sender === 'user' ? 'justify-end' : 'justify-start'}">
  <div class="message-container w-[80%] max-h-[300px] overflow-y-auto rounded-lg p-3 border border-black/10 shadow-sm {message.sender === 'user' ? 'bg-teal-100 dark:bg-teal-800 text-primary-foreground' : message.error ? 'bg-destructive/10 text-destructive dark:bg-destructive/20' : 'bg-slate-100 dark:bg-slate-800'} scrollbar-thin scrollbar-track-transparent scrollbar-thumb-black/20 dark:scrollbar-thumb-white/20 dark:border-white/10">
    <div class="whitespace-pre-wrap break-words">
      {#if message.isLoading}
        <div class="flex items-center gap-2">
          <Icon src={ArrowPathRoundedSquare} class="animate-spin h-4 w-4" />
          <span>Thinking...</span>
        </div>
      {:else}
        {@html message.sender === 'bot' ? processMarkdown(message.text) : message.text}
      {/if}
    </div>
    {#if message.timestamp}
      <div class="text-xs mt-1 text-gray-500 dark:text-gray-300">
        {message.timestamp}
      </div>
    {/if}
    {#if message.error}
      <div class="flex items-center gap-1 text-xs text-destructive mt-1">
        <Icon src={ExclamationCircle} class="h-4 w-4" />
        <span>Error occurred</span>
      </div>
    {/if}
  </div>
</div>