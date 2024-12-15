<script lang="ts">
  import Header from "../components/header/Header.svelte";
  import "../app.css";

  let { children } = $props();
</script>

<div class="hidden flex-col md:flex">
  <div class="border-b">
    <div class="flex h-16 items-center px-4">
		<Header />
    </div>
  </div>


	<main class="p-4">
	  {@render children()}
	</main>
  
</div>

