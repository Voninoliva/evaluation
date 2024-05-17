DO $$
DECLARE table_record RECORD;
BEGIN FOR table_record IN
        SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE'
    LOOP
        EXECUTE format('TRUNCATE TABLE %I RESTART IDENTITY CASCADE', table_record.table_name);
    END LOOP;END $$;
