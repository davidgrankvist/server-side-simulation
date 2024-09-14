@echo off

ffmpeg -f rawvideo -pix_fmt rgba -s 800x800 -r 30 -i tcp://127.0.0.1:12345 -f sdl "Video Display"
