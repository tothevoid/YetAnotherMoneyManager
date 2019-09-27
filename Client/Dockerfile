FROM node:latest as build-stage

WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY . .
RUN npm run build
CMD ["serve", "-p", "80", "-s", "."]