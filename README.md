# server-side-simulation

Server side simulation.

## About

This project is about rendering a simulation server side and stream it as video to clients. The idea is that clients with limited hardware can view the simulation without running it on their own.

### Overview

This is a work in progress, but here is the general idea.

```
Simulation -> Encoder -> Server -> Client
```

Steps:
- The simulation renders a bitmap to an off screen buffer
- The bitmaps are passed through an encoder
- The server listens to the stream of encoded bitmaps and forwards them to connected clients
- The client decodes and renders the bitmaps

### Docs

To see some ideas I tried out, see [Log.md](./Docs/Log.md).
