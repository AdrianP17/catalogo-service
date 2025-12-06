import http from 'k6/http';
import { check, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import { FormData } from 'https://jslib.k6.io/formdata/0.0.2/index.js';

// --- Configuración de la Prueba ---
export const options = {
  stages: [
    { duration: '10s', target: 5 },  // 1. Ramp-up: Aumentar a 5 usuarios virtuales en 10 segundos
    { duration: '20s', target: 5 },  // 2. Carga Sostenida: Mantener 5 usuarios durante 20 segundos
    { duration: '5s', target: 0 },   // 3. Ramp-down: Bajar a 0 usuarios en 5 segundos
  ],
  thresholds: {
    'http_req_duration': ['p(95)<2000'], // El 95% de las solicitudes deben completarse en menos de 2000ms (2s)
    'http_req_failed': ['rate<0.01'],   // La tasa de fallos debe ser inferior al 1%
  },
};

// --- Datos de Prueba ---
// Cargar los archivos de imagen una sola vez y compartirlos entre los VUs
// const image1 = open('./data/image1.jpg', 'b');
// const image2 = open('./data/image2.jpg', 'b');
const csvFile = open('./productos.csv');

// URL del endpoint (ajusta el puerto si es necesario)
const API_URL = 'http://localhost:5223/api/productos/carga-masiva';

// --- Lógica de la Prueba ---
export default function () {
  // Construir el cuerpo de la petición como 'multipart/form-data'
  const fd = new FormData();
  
  fd.append('archivoCsv', http.file(csvFile, 'productos.csv', 'text/csv'));
  // fd.append('imagenes', http.file(image1, 'image1.jpg', 'image/jpeg'));
  // fd.append('imagenes', http.file(image2, 'image2.jpg', 'image/jpeg'));

  // Enviar la petición POST
  const res = http.post(API_URL, fd.body(), {
    headers: { 'Content-Type': 'multipart/form-data; boundary=' + fd.boundary },
  });

  // Verificar la respuesta
  check(res, {
    'Respuesta exitosa (código 200)': (r) => r.status === 200,
    'Respuesta sin errores (código 500)': (r) => r.status !== 500,
  });

  if (res.status !== 200) {
    console.error(`Request failed! Status: ${res.status}, Body: ${res.body}`);
  }

  sleep(1); // Esperar 1 segundo entre iteraciones
}
