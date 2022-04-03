FROM node:14 AS build
WORKDIR /usr/src/app

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build

FROM nginx
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
COPY --from=build /usr/src/app/dist .
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]