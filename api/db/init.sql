-- This should be moved into seeders if it's useful. It's not necessary anymore though.
CREATE DATABASE IF NOT EXISTS GestrApp;

INSERT IGNORE INTO Presets (name, userId) VALUES ('GPS', 'mock-user-id');
INSERT IGNORE INTO Presets (name, userId) VALUES ('Music', 'mock-user-id');
INSERT IGNORE INTO Presets (name, userId) VALUES ('Phone', 'mock-user-id');

INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('A', (SELECT id FROM Presets WHERE name = 'GPS'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('B', (SELECT id FROM Presets WHERE name = 'GPS'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('C', (SELECT id FROM Presets WHERE name = 'GPS'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('D', (SELECT id FROM Presets WHERE name = 'GPS'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('E', (SELECT id FROM Presets WHERE name = 'GPS'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('F', (SELECT id FROM Presets WHERE name = 'GPS'));

INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('A', (SELECT id FROM Presets WHERE name = 'Music'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('B', (SELECT id FROM Presets WHERE name = 'Music'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('C', (SELECT id FROM Presets WHERE name = 'Music'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('D', (SELECT id FROM Presets WHERE name = 'Music'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('E', (SELECT id FROM Presets WHERE name = 'Music'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('F', (SELECT id FROM Presets WHERE name = 'Music'));

INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('A', (SELECT id FROM Presets WHERE name = 'Phone'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('B', (SELECT id FROM Presets WHERE name = 'Phone'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('C', (SELECT id FROM Presets WHERE name = 'Phone'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('D', (SELECT id FROM Presets WHERE name = 'Phone'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('E', (SELECT id FROM Presets WHERE name = 'Phone'));
INSERT IGNORE INTO GestureSettings (gesture, presetId) 
  VALUES ('F', (SELECT id FROM Presets WHERE name = 'Phone'));
