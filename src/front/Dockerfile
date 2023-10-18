# Use uma imagem base Node.js
FROM node:14

# Defina o diretório de trabalho no contêiner
WORKDIR /app

# Copie os arquivos package.json e package-lock.json para o contêiner
COPY ./front/package*.json /app

# Instale as dependências do npm
RUN npm install

# Copie todos os outros arquivos do aplicativo para o contêiner
COPY ./front /app

# Compile o aplicativo React
RUN npm run build

# Exponha a porta em que o aplicativo estará em execução (pode ser personalizada)
EXPOSE 3000

# Comando para iniciar a aplicação quando o contêiner for executado
CMD ["npm", "start"]
