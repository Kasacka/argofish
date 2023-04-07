FROM node:16
WORKDIR /usr/src/app
COPY package*.json ./
RUN npm install
COPY . .
EXPOSE 8080
ENV SERVICE_PORT=8080
ENV SERVICE_VERSION=1.434
CMD [ "node", "index.js" ]