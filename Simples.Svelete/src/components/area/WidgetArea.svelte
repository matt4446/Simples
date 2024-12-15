<script lang="ts">
    import { createSwapy, type Swapy } from 'swapy';
    import { onDestroy, onMount } from 'svelte';

    let container:HTMLElement;
    let swapy:Swapy|null = null;
  
    onMount(() => {
      // If container element is loaded
      if (container) {
        swapy = createSwapy(container)
  
        // Your event listeners
        swapy.onSwap(event => {
          console.log('swap', event)
        })
      }
    })
  
    onDestroy(() => {
      // Destroy the swapy instance on component destroy
      swapy?.destroy()
    })
  </script>

<div class="flex gap-4 border-red-300 border-2" bind:this={container}>  
  <slot></slot>
</div>