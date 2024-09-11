@echo off

ffmpeg -f mp4 -i tcp://127.0.0.1:12345 -f sdl "Video Display"
