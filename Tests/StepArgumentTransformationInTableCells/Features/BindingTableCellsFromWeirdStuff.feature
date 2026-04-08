Feature: Bind table cells from weird inputs

Tests that step argument transformations can be applied to table cells
to convert glyphs or words into enums or booleans.

@table-cell-transformations
Scenario: Bind enums, dates an booleans from glyphs or words in table cells
	Given the issues
		| ID | Kind | Title       | Closed | Created    |
		|  1 | 🐛   | 1st bug     | yes    | 2 days ago |
		|  2 | bug  | 2nd bug     | no     | 1 day ago  |
		|  3 | 🦗   | 3rd bug     | ☑      | 1 day ago  |
		|  5 | 🧩   | 1st feature | ☐      | today      |
		|  8 | ⚙    | 1st chore   | no     | now        |
	Then bug 1 '1st bug' (created 2 days ago) should be closed
	And bug 2 '2nd bug' (created 1 day ago) should be open
	And bug 3 '3rd bug' (created 1 day ago) should be closed
	And feature 5 '1st feature' (created today) should be open
	And chore 8 '1st chore' (created now) should be open
