@echo off

ffmpeg -f mpegts -i tcp://127.0.0.1:12345\?listen -f sdl "Video Display"
