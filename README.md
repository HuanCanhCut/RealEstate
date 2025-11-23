# SETUP MÔI TRƯỜNG

1. Copy file `.env.example` thành `.env` hoặc `.env.development`

2. Sửa lại các giá trị trong file `.env.development` hoặc `.env` (bao gồm `JWT_SECRET_KEY`, `DB_USER`, `DB_PASSWORD`, ...)

## THIẾT LẬP BIẾN MÔI TRƯỜNG

Các giá trị nhạy cảm như JWT_SECRET_KEY hoặc DB_PASSWORD ... nên được lưu trữ trong các file `env` và phải được đưa vào .gitignore
