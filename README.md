# server-side-simulation

Server side simulation.

## About

This project is about rendering a simulation server side and stream it as video to clients. The idea is that clients with limited hardware can view the simulation without running it on their own.

### Overview

This is a work in progress, but here is the general idea.

```mermaid
---
config:
  layout: elk
  look: handDrawn
  theme: dark
---
flowchart LR
  Simulation -->|"Bitmaps"| Encoder
  Encoder -->|"Frames"| Server
  Server -->|"Frames"| Client
```

Steps:
- The simulation renders a bitmap to an off screen buffer
- The bitmaps are passed through an encoder
- The server listens to the stream of encoded bitmaps and forwards them to connected clients
- The client decodes and renders the bitmaps

### Docs

For more architecture details and diagrams, see [Architecture.md](./Docs/Architecture.md).

To see some ideas I have tried out, see [Log.md](./Docs/Log.md).

## Resources

ThePrimeagen has a [great video](https://www.youtube.com/watch?v=3f9tbqSIm-E) about rendering Doom server side.
