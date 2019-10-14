FROM node:latest as build-stage
WORKDIR /app
COPY package*.json /app/
RUN npm install

ARG PROD="production"
ENV REACT_APP_STAGE $PROD

COPY . .
RUN npm run build
CMD ["serve", "-p", "80", "-s", "."]