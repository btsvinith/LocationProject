INSERT INTO Country (CountryName, RequiresStateDistrict)
VALUES 
('India', 1),
('United States', 1),
('Australia', 1),
('Singapore', 0);

-- India States
INSERT INTO State (StateName, CountryId)
VALUES 
('Tamil Nadu', 1),
('Karnataka', 1),
('Maharashtra', 1);

-- USA States
INSERT INTO State (StateName, CountryId)
VALUES 
('California', 2),
('Texas', 2);

-- Australia States
INSERT INTO State (StateName, CountryId)
VALUES 
('New South Wales', 3),
('Victoria', 3);

 -- Tamil Nadu Districts
INSERT INTO District (DistrictName, StateId)
VALUES 
('Chennai', 1),
('Coimbatore', 1),
('Madurai', 1);

-- Karnataka Districts
INSERT INTO District (DistrictName, StateId)
VALUES 
('Bangalore Urban', 2),
('Mysore', 2);

-- Maharashtra Districts
INSERT INTO District (DistrictName, StateId)
VALUES 
('Mumbai', 3),
('Pune', 3);

-- California Districts
INSERT INTO District (DistrictName, StateId)
VALUES 
('Los Angeles County', 4),
('San Diego County', 4);

-- Texas Districts
INSERT INTO District (DistrictName, StateId)
VALUES 
('Harris County', 5),
('Dallas County', 5);

-- Australia States Districts
INSERT INTO District (DistrictName, StateId)
VALUES 
('Sydney', 6),
('Newcastle', 6),
('Melbourne', 7),
('Geelong', 7);