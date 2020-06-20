[TOC]

# 安装.net core
本人开发环境为ubunu 20.04 ✖64位  阿里云镜像源

官网安装教程链接: [ubuntu安装.net core 3.1](https://docs.microsoft.com/zh-cn/dotnet/core/install/linux-ubuntu#2004-)

1.安装镜像源

`wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
`

2.运行按扎ung命令,自动下载资源并安装

`sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-3.1
`

3.(可选)安装.net core运行时

`sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y aspnetcore-runtime-3.1
`

 `
 #个人理解就是为了方便部署和运行软件,一般地我们的程序都会经过编译部署在机器上,这时程序都被编译成了
 二进制文件,所以.net core就没有必要安装了,只需要运行时即可,减少一部分存储量
 `

 # 创建webapi项目模板

默认在当前目录下创建文件,项目名称就是文件夹的名称  `dotnet new webapi`
