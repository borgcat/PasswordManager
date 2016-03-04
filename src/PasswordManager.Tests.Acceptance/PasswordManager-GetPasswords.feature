Feature: PasswordManager-GetPasswords
	In order to avoid remembering passwords
	As a forgetfull person 
	I can remember a single password or secret
	I want to get a list of my passwords for all my secured websites I am a member of

@GetPasswords
Scenario: Get A List Of Passwords
	Given I have a secret of 'Test1234'
	Then the result should be count of passwords greater than zero
