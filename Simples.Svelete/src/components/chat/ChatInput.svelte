<script lang="ts">
  import { Textarea } from "$lib/components/ui/textarea";
  import Button from "$lib/components/ui/button/button.svelte";
  import { Icon, ArrowPathRoundedSquare, PaperAirplane } from "svelte-hero-icons";

  let newMessage: string = "";
  export let isLoading: boolean;
  export let handleKeydown: (event: KeyboardEvent) => void;
  export let sendMessage: (text: string) => void;

  const handleSend = () => {
    if (newMessage?.trim()) {
      sendMessage(newMessage);
      newMessage = "";
    }
  };
</script>

<div class="relative w-full max-w-full justify-self-center flex gap-4">
  <Textarea
    bind:value={newMessage}
    onkeydown={handleKeydown}
    placeholder="Type a message... (Press Enter to send)"
    class="block resize-none rounded-md border-0 p-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:outline-none sm:text-sm/6 [field-sizing:content] dark:text-gray-100 dark:bg-gray-800 dark:ring-gray-600"
    rows={1}
  />
  <Button onclick={handleSend}>
    {#if isLoading}
      <Icon src={ArrowPathRoundedSquare} class="animate-spin mr-2" />
      Sending...
    {:else}
      <Icon src={PaperAirplane} class="mr-2" />
      Send
    {/if}
  </Button>
</div>