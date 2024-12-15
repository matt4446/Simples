<script lang="ts">
    import * as Card from "$lib/components/ui/card";
  import WidgetArea from "../../components/area/WidgetArea.svelte";
</script>
  
<WidgetArea>
  <div data-swapy-slot="a">
    <div data-swapy-item="a">
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

  </div>

  <div data-swapy-slot="b">
    <div data-swapy-item="b">
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
  </div>

</WidgetArea>
  
