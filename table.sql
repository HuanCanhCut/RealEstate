CREATE TABLE users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    full_name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    nickname VARCHAR(50) NOT NULL UNIQUE,
    phone_number VARCHAR(10),
    avatar VARCHAR(255),
    role ENUM('user', 'agent', 'admin') DEFAULT 'user',
    address VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);


CREATE TABLE categories (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE posts (
    id INT PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    address VARCHAR(255) NOT NULL,
    administrative_address VARCHAR(255) NOT NULL,
    project_type ENUM('sell', 'rent'),
    images TEXT,
    post_status ENUM(
        'approved',
        'pending',
        'rejected'
    ) DEFAULT 'pending',
    status ENUM(
        'Chưa bàn giao',
        'Đã bàn giao'
    ) DEFAULT 'Chưa bàn giao',
    category_id INT NOT NULL,
    user_id INT NOT NULL,
    role ENUM('user', 'agent') DEFAULT 'user',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE post_details (
    id INT PRIMARY KEY AUTO_INCREMENT,
    post_id INT NOT NULL,
    bedrooms int,
    bathrooms int,
    balcony VARCHAR(20),
    main_door VARCHAR(20),
    legal_documents VARCHAR(50),
    interior_status VARCHAR(50),
    area int,
    price DECIMAL(14, 2),
    deposit DECIMAL(14, 2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (post_id) REFERENCES posts (id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE favorites (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    post_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (post_id) REFERENCES posts (id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE contracts (
    id INT PRIMARY KEY AUTO_INCREMENT,
    customer_id INT NOT NULL,
    agent_id INT NOT NULL,
    customer_cccd VARCHAR(12) NOT NULL,
    customer_phone VARCHAR(12) NOT NULL,
    post_id INT NOT NULL,
    amount DECIMAL(14, 2) NOT NULL,
    commission DECIMAL(14, 2) NOT NULL,
    status ENUM(
        'pending',
        'approved',
        'rejected'
    ) DEFAULT 'pending',
    duration ENUM('2 năm', '5 năm', '10 năm') DEFAULT '2 năm',
    clause TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (post_id) REFERENCES posts (id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (agent_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE FULLTEXT INDEX full_name_idx ON users (full_name);

CREATE FULLTEXT INDEX nickname_idx ON users (nickname)

ALTER TABLE post_details MODIFY price DECIMAL(14, 2);

ALTER TABLE post_details MODIFY deposit DECIMAL(14, 2);

ALTER Table contracts MODIFY amount DECIMAL(14, 2);

ALTER Table contracts MODIFY commission DECIMAL(14, 2);

CREATE FULLTEXT INDEX post_title_address_idx ON posts (
    title,
    address,
    administrative_address
);

ALTER TABLE categories ADD COLUMN `key` VARCHAR(255);

ALTER TABLE categories
modify column `key` VARCHAR(255) NOT NULL UNIQUE;

CREATE TABLE refresh_tokens (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    token VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE blacklisted_tokens (
    id INT PRIMARY KEY AUTO_INCREMENT,
    token VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

ALTER TABLE categories ADD COLUMN user_id INT NOT NULL;

ALTER TABLE categories ADD FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE;

create index category_name_key_idx on categories (`name`, `key`);