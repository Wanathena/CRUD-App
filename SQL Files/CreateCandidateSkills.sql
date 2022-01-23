create table dbo.Candidate_skill(
	candidate_id int NOT NULL,
	skill_id int NOT NULL,
	FOREIGN KEY (candidate_id)
		REFERENCES dbo.Candidates (candidate_id)
		ON DELETE CASCADE,
	FOREIGN KEY (skill_id)
		REFERENCES dbo.Skills (skill_id)
		ON DELETE CASCADE
	)