<script lang="ts">
    import type { Message } from "../../types";
    import MessageItem from "./MessageItem.svelte";

    export let messages: Message[] = [];
    export let processMarkdown: (text: string) => string;
</script>

<div class="space-y-4">
  {#if messages.length === 0}
    <p class="text-gray-500 text-center italic">No messages yet. Start the conversation!</p>
  {/if}

  {#each messages as message}
    <MessageItem {message} {processMarkdown} />
  {/each}
</div>