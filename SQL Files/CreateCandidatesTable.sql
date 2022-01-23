create table dbo.Candidates(
candidate_id int identity(1, 1),
candidate_name nvarchar(50),
date_of_birth date,
contact_number nvarchar(50),
email nvarchar(50)
)
