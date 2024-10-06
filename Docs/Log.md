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

### Run-Length Encoding

Run-Length Encoding (RLE) is based on the simple idea that repeated values can be replaced with a counter and a value. For example `0 0 0 0` can be encoded as `4 0`.

When used in conjunction with XOR deltas, static values become easier to compress.

Example:

Let's say that two consecutive frames have these values.

```
F1 = 1 0 1 0 1 0 1 0 1 0 1 0 1 0
F2 = 9 0 1 0 1 0 1 0 1 0 1 0 1 0
```

There are no sequences of repeated values, but only one value changed between the frames. The XOR delta becomes this

```
8 0 0 0 0 0 0 0 0 0 0 0 0 0
```

Which can be encoded with RLE as

```
1 8 13 0
```

#### Storing the count

There is a trade-off in RLE: longer runs can compress more data, but also require more bytes to store large counts. If one-byte counters are used then the run length is at most `2^8 - 1 = 255` and each run requires two bytes. If two-byte counters are used then the run length is at most `2^16 - 1 = 65535` and each run is 3 bytes long.

Example:

Let's say that we have an 800x800 bitmap of indexed one-byte colors. That's `800 * 800 = 640000` bytes. In the best case, the entire bitmap has the same color. With one-byte runs, the bitmap can be compressed to `2 * 640000 / 255 ~ 5000`. With two-byte runs it can be compressed to `3 * 640000 / 65535 ~ 30`.