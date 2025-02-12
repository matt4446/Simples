<script lang="ts">
  import * as Card from "$lib/components/ui/card";
  import * as ScrollArea from "$lib/components/ui/scroll-area";
</script>

<svelte:head>
  <title>Welcome to Simples</title>
  <meta name="description" content="Home dashboard for Simples application" />
</svelte:head>

<div class="container mx-auto p-4">
  <h1 class="text-4xl font-bold mb-6">Welcome to Simples</h1>
  
  <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
    <Card.Root class="shadow-lg">
      <Card.Header>
        <Card.Title>Quick Actions</Card.Title>
        <Card.Description>Frequently used controls and shortcuts</Card.Description>
      </Card.Header>
      <Card.Content>
        <ScrollArea.Root class="h-[200px]">
          <div class="space-y-4">
            <button class="w-full p-3 bg-primary text-primary-foreground rounded-md hover:bg-primary/90">
              Home Automation
            </button>
            <button class="w-full p-3 bg-secondary text-secondary-foreground rounded-md hover:bg-secondary/90">
              Chat Interface
            </button>
            <button class="w-full p-3 bg-secondary text-secondary-foreground rounded-md hover:bg-secondary/90">
              Device Status
            </button>
          </div>
        </ScrollArea.Root>
      </Card.Content>
    </Card.Root>

    <Card.Root class="shadow-lg">
      <Card.Header>
        <Card.Title>System Status</Card.Title>
        <Card.Description>Current system information and metrics</Card.Description>
      </Card.Header>
      <Card.Content>
        <div class="space-y-4">
          <div class="flex justify-between items-center">
            <span>API Service</span>
            <span class="px-2 py-1 bg-green-100 text-green-800 rounded">Online</span>
          </div>
          <div class="flex justify-between items-center">
            <span>Home Assistant</span>
            <span class="px-2 py-1 bg-green-100 text-green-800 rounded">Connected</span>
          </div>
          <div class="flex justify-between items-center">
            <span>Chat Service</span>
            <span class="px-2 py-1 bg-green-100 text-green-800 rounded">Ready</span>
          </div>
        </div>
      </Card.Content>
    </Card.Root>
  </div>
</div>
