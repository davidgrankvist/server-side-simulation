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

## Idea 2.1 - Optimizations

Every nth frame sent by the simulation is a keyframe and the others are deltas. The frame has a header so that keyframe detection is trivial.

Frame format:
```
----------------------------------
| version | type | length | data |
----------------------------------
```

The challenge is to make the `data` portion as small as possible.

### Indexed colors

Instead of sending RGBA pixels, the simulation can use a restricted color palette. Each color can be represented as a single byte index into the palette. This makes the bitmap 25% of the original size, including for keyframes. The drawback is that we limit ourselves to 256 colors.

### Delta encoding and RLE

This part is inspired by ThePrimeagen's Doom video.

Instead of sending entire bitmaps each frame, we can send an initial larger frame (I-Frame) and subsequent smaller delta frames with the updates (P-Frame). The delta can be calculated using XOR, which on the client side can be decoded with XOR as well. Using only XOR will not make the frame smaller, but if the difference between frames is small then there will be a lot of zeros. This makes the P-Frames easier to compress with for example Run-Length Encoding (RLE).

The XOR decoding step works out because of the properties of XOR (associativity).

Example:

The simulation produces two frames, F1 and F2. The server decides to use F1 as an I-frame and F2 as an P-Frame. Instead of sending F2 directly, it computes the delta FX.

```
FX = F1 XOR F2
```

The client receives F1 and FX. It reconstructs F2 using XOR.

```
F1 XOR FX = F1 XOR (F1 XOR F2) = (F1 XOR F1) XOR F2 = 0 XOR F2 = F2
```
