  // Import the functions you need from the SDKs you need
  import { initializeApp } from "https://www.gstatic.com/firebasejs/12.17.1/firebase-app.js";
  import { getDatabase } from "https://www.gstatic.com/firebasejs/12.17.1/firebase-database.js";
  // TODO: Add SDKs for Firebase products that you want to use
  // https://firebase.google.com/docs/web/setup#available-libraries

  // Your web app's Firebase configuration
  // For Firebase JS SDK v7.20.0 and later, measurementId is optional
  const firebaseConfig = {
    apiKey: "AIzaSyD6tR2FKsAVkUkAP6bKUDYnf17kmp8RIzg",
    authDomain: "digitalparking-28373.firebaseapp.com",
    projectId: "digitalparking-28373",
    storageBucket: "digitalparking-28373.firebasestorage.app",
    messagingSenderId: "815552643085",
    appId: "1:815552643085:web:633db398d852cf69af1544",
    measurementId: "G-058J7M060R"
  };

  // Initialize Firebase
  const app = initializeApp(firebaseConfig);
  const database = getDatabase(app);

  export { database };