# Converting a Twitch stream to a timelapse

```
D:\downloads\ffmpeg\bin\ffmpeg.exe -i
    D:\Downloads\437356383-51151183-121a6d34-b369-4866-beae-663b0cb40cca.mp4
    -filter:v "setpts=0.1*PTS"
    -an
    D:\Captures\output.mp4
```
