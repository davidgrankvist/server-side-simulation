@echo off

ffmpeg -f mpegts -i tcp://127.0.0.1:12345 -f sdl "Video Display"
