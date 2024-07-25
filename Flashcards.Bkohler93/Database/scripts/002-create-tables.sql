CREATE TABLE Stacks(
    id INTEGER PRIMARY KEY NOT NULL,
    name VARCHAR(255)
);

CREATE TABLE Flashcards (
    id INT NOT NULL PRIMARY KEY,
    stack_id INT NOT NULL,
    front VARCHAR(255),
    back VARCHAR(255),
    FOREIGN KEY (stack_id) REFERENCES Stacks(id)
);

CREATE TABLE StudySessions (
    id INT NOT NULL PRIMARY KEY,
    stack_id INT NOT NULL,
    study_time DATETIME,
    score INT,
    FOREIGN KEY (stack_id) REFERENCES Stacks(id)
);