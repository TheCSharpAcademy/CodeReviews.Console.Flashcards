SELECT 
    stacks.Id,
    stacks.name,
    COUNT(study_sessions.Id) AS session_count
FROM 
    stacks
LEFT JOIN 
    study_sessions ON stacks.Id = study_sessions.stackId
GROUP BY 
    stacks.Id, stacks.name;