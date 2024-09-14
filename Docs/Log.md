# Log

The following log entries describe different approaches to this project.

## Idea 1

Encode the bitmaps as video.

```
Simulation -> ffmpeg (video) -> Server -> Client
```

One downside of this is that decoding becomes complex for clients that connect in the middle of the simulation.

The server receives video fragments consisting of multiple frames, but a newly connected client needs the first frame to be a keyframe. This requires the server to either send keyframes frequently (inefficient) or to detect keyframes within fragments and output modified fragments (complex).

## Idea 2

Use a custom binary encoding.

```
Simulation -> custom encoder -> Server -> Client
```

One benefit of this approach is that it can start simple and send entire bitmaps. The same pipeline is easy to extend with more efficient encoding schemes.

If keyframe detection is needed, the custom format can have a simple header containing the frame type.
