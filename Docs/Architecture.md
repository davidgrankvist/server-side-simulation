# Architecture

## Diagrams

### Simple

```mermaid
---
config:
  layout: elk
  look: handDrawn
  theme: dark
---
flowchart TB
  Simulation -->|"Bitmaps (In-process)"| Encoder
  Encoder -->|"Frames (TCP)"| Server
  Server -->|"Frames (Websocket)"| Client1
  Server -->|"Frames (Websocket)"| Client2
  Server -->|"Frames (Websocket)"| Client3
```

### With multiplexer scaling

```mermaid
---
config:
  layout: elk
  look: handDrawn
  theme: dark
---
flowchart TB
  Simulation -->|"Bitmaps (In-process)"| Encoder
  Encoder -->|"Frames (TCP)"| Server
  Server -->|"Frames (Websocket)"| Multiplexer1
  Server -->|"Frames (Websocket)"| Multiplexer2
  Multiplexer1 -->|"Frames (Websocket)"| Client1
  Multiplexer1 -->|"Frames (Websocket)"| Client2
  Multiplexer2 -->|"Frames (Websocket)"| Client3
```

### Client-server interaction

```mermaid
sequenceDiagram
    Client->>Server: Requests web client
    Server->>Client: Sends HTML/JS
    Client->>Server: Connects Websocket
    Server->>Client: Sends frames
    Client->>Client: Decodes/Renders
```

## Protocol

### Frame format

```
----------------------------------
| version | type | length | data |
----------------------------------
```

### Encoding scheme

```mermaid
---
config:
  layout: elk
  look: handDrawn
  theme: dark
---
flowchart LR
  Bitmap --> IC[Indexed Colors]
  IC --> D[XOR Delta Encoding]
  D --> R[Run-Length Encoding]
```

### Late joiners

Every nth frame is an I-Frame and the rest are P-Frames. On a new client connection, the server will wait for the next I-Frame before it starts forwarding frames to the client. This allows clients to connect at any point during the simulation.