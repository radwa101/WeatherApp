import * as fetch from "isomorphic-fetch";

// let post = (body, url) => {
//   let { SERVICES } = getConfig().APP;
//   let { HEADERS } = getConfig().APP;
//   return fetch(SERVICES.CARRIER.SETTINGS + url, {
//     method: "POST",
//     headers: setHeader(HEADERS),
//     body: JSON.stringify(body),
//   });
// };

// let put = (body: any = null, url: string) => {
//   let { SERVICES } = getConfig().APP;
//   let { HEADERS } = getConfig().APP;
//   return fetch(SERVICES.CARRIER.SETTINGS + url, {
//     method: "PUT",
//     headers: setHeader(HEADERS),
//     body: (body == null) ? "" : JSON.stringify(body),
//   });
// };



let head = new Headers();
head.set("Content-Type", "application/json");
head.set("Accept", "application/json");
head.set("Access-Control-Allow-Origin", "*");

let get = (url) => {
 return fetch(url, {
    method: "GET",
    mode: "cors",
    header: head
  });
};
// let setHeader = (HEADERS: any) => {
//   let head = new Headers();
//       head.set("Content-Type", "application/json");
//       head.set(HEADERS.CARRIER_API.AUTH_USER.KEY, HEADERS.CARRIER_API.AUTH_USER.VALUE);
//       head.set(HEADERS.CARRIER_API.AUTH_TOKEN.KEY, HEADERS.CARRIER_API.AUTH_TOKEN.VALUE);
//   return head;
// };

let utilsRepo = {
  //post,
  get,
  //put
};
export default utilsRepo;