export const environment = {
  production: false,
  // Use the API's HTTPS profile (see Properties/launchSettings.json ->
  // "https" -> https://localhost:portno). The session cookie is configured as
  // SameSite=None; Secure so the browser will only send it back on HTTPS
  // cross-origin calls from the Angular dev server (http://localhost:portno).
  apiUrl: 'https://localhost:portno/api'
};
