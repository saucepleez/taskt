param($message="You didn't enter one. Use -message.", $timeout=5)

[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")

$oReturn=[System.Windows.Forms.Messagebox]::Show($message)

write-host "Sample Powershell Script, here's an argument:"
write-host $message

start-sleep -s $timeout
