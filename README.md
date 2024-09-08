# server-side-simulation

Server side simulation.

## About

This project is about rendering a simulation server side and stream it as video to clients. The idea is that clients with limited hardware can view the simulation without running it on their own.

### Overview

This is a work in progress, but here is the general idea.

```
Simulation -> Video Encoder -> Server -> Client
```

Steps:
- The simulation renders a bitmap to an off screen buffer
- The bitmaps are passed to a video encoder that outputs a video stream
- The server listens to the video stream and forwards it to connected clients
- The client renders the video stream
