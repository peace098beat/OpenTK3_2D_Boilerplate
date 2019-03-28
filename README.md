# OpenTK3 Samples - 2D編 -

## 概要
OpenTK 3.0 説明書

OpenTK3.0がリリースされたので、サンプルコードを書いた.

2D編.


## インストールの方法

1. nugetを使ってOpenTK(3.0)をインストールする. 以上
2. OpenALを使う場合は, 事前に[OpenALをインストール](https://www.openal.org/)しておく.


## 参考になるドキュメント

1. [github/opentk](https://github.com/opentk/opentk)
2. [OpenTK API Reference](https://opentk.net/api/index.html)
3. [opentk/LearnOpenTK: A port of learnopengl.com's tutorials to OpenTK and C#.](https://github.com/opentk/LearnOpenTK)
4. [GLUTによる「手抜き」OpenGL入門](https://tokoik.github.io/opengl/libglut.html)

# OpenTK API メモ

## OpenTK.Audio
OpenTK.AudioはOpenTK.Audio.OpenALのラッパークラスライブラリ. 
OpenALはOpenTK.Audio.OpenALでラップされている.
おもに、AudioCapture(録音)とAudioContext(再生)の2種類.

## OpenTK.GameWindow
NativeWindowを継承しているクラス.
OpenGLのウィンドウを作る