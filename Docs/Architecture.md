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

Frame format:
```
----------------------------------
| version | type | length | data |
----------------------------------
```