# Project 2 | HTML Serializer

## Project Overview
This project is a tool for parsing and processing HTML files using C#. It is composed of two main parts:

1. **HTML Serializer** – Converts HTML code into an object tree.
2. **HTML Query** – Allows querying the HTML tree using CSS-like selectors.

This kind of tool serves as the foundation for building a Crawler (web scraper), similar to those used by search engines or any service that extracts data from websites.

---

## Technologies Used
- Programming Language: C#
- Libraries: `System.Net.Http`, `System.Text.Json`, `Regex`
- Design Patterns: Singleton, Tree structures, HTML parsing

---

## How to Run
1. Download JSON files with tag definitions (including self-closing tags).
2. Ensure they're placed in the correct input directory.
3. Run the `Load()` function with a valid website URL.
4. Parse the HTML and build the tree.
5. Perform searches using the `Selector` class.
