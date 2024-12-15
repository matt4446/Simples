<script lang="ts">
  import * as Card from "$lib/components/ui/card";
</script>

<svelte:head>
  <title>Home</title>
  <meta name="description" content="Test" />
</svelte:head>

<div class="text-column">

  <Card.Root>
    <Card.Header>
      <Card.Title>Card Title</Card.Title>
      <Card.Description>Card Description</Card.Description>
    </Card.Header>
    <Card.Content>
      <p>Card Content</p>
    </Card.Content>
    <Card.Footer>
      <p>Card Footer</p>
    </Card.Footer>
  </Card.Root>
</div>
