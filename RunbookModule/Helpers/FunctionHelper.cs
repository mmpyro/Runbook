namespace RunbookModule.Helpers
{
    public static class FunctionHelper
    {
        public static string CheckOperationStatus()
        {
            return @"
                function Check-OperationStatus() {
                    param($msg)
                    if($? -eq $false ) {
                        Write-Host 'git reset --hard fail'
                        throw $msg
                    }
                }
            ";
        }
    }
}
