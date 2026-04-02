Feature: Register converter for different contexts

Tests that registering converter for two different contexts works.

@table-cell-transformations
Scenario: Register converter for two different scenario contexts
	Given the issues
		| ID | Kind | Title       | Closed | Created    |
		|  1 | 🐛   | 1st bug     | yes    | 2 days ago |
		|  2 | bug  | 2nd bug     | no     | 1 day ago  |
		|  3 | 🦗   | 3rd bug     | ☑      | 1 day ago  |
		|  5 | 🧩   | 1st feature | ☐      | today      |
		|  8 | ⚙    | 1st chore   | no     | now        |
	And the date ranges
		| ID | Start        | End          |
		|  1 | 164 days ago | 162 days ago |
		|  2 | in 10 days   | in 14 days   |
		|  5 | in 72 days   | in 78 days   |
		|  8 | in 50 days   | in 51 days   |
	Then date range 1 should start 164 days ago and end 162 days ago
	And date range 2 should start in 10 days and end in 14 days
	And date range 5 should start in 72 days and end in 78 days
	And date range 8 should start in 50 days and end in 51 days
	And bug 1 '1st bug' (created 2 days ago) should be closed
	And bug 2 '2nd bug' (created 1 day ago) should be open
	And bug 3 '3rd bug' (created 1 day ago) should be closed
	And feature 5 '1st feature' (created today) should be open
	And chore 8 '1st chore' (created now) should be open
