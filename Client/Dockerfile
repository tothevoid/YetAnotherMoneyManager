FROM node:latest as build-stage

WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY . .
RUN npm run build

EXPOSE 8080
CMD ["serve", "-s", "dist", "-p", "8080"]