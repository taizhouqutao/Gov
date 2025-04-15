# 使用官方.NET Core运行时镜像作为基础镜像
FROM mcr.microsoft.com/dotnet/sdk:8.0
 
# 设置工作目录
WORKDIR /backend
 
# 复制项目发布文件到容器中的工作目录
COPY ./backend/ .

COPY ./backend/simsun.ttc /usr/share/fonts/
 
# 设置环境变量，端口
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
 
# 当容器启动时，运行应用程序
ENTRYPOINT ["dotnet", "WebBackend.dll"]
