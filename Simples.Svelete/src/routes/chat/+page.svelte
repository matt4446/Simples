<script lang="ts">
    import Chat from '../../components/chat/Chat.svelte';
import type { PageData } from './$types';

    let { data }: { data: PageData } = $props();
</script>

<Chat></Chat>
