using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballManagement.Data
{
    public static class ExtraMigration
    {
        public static void Steps(MigrationBuilder migrationBuilder)
        {
            ////Patient Table Triggers for Concurrency
            //migrationBuilder.Sql(
            //    @"
            //        CREATE TRIGGER SetLeagueTimestampOnUpdate
            //        AFTER UPDATE ON League
            //        BEGIN
            //            UPDATE Leagues
            //            SET RowVersion = randomblob(8)
            //            WHERE rowid = NEW.rowid;
            //        END
            //    ");
            //migrationBuilder.Sql(
            //    @"
            //        CREATE TRIGGER SetLeagueTimestampOnInsert
            //        AFTER INSERT ON Leagues
            //        BEGIN
            //            UPDATE Leagues
            //            SET RowVersion = randomblob(8)
            //            WHERE rowid = NEW.rowid;
            //        END
            //    ");

            //Player Table Triggers for Concurrency
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetPlayerTimestampOnUpdate
                    AFTER UPDATE ON Players
                    BEGIN
                        UPDATE Players
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetPlayerTimestampOnInsert
                    AFTER INSERT ON Players
                    BEGIN
                        UPDATE Players
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
        }
    }
}
