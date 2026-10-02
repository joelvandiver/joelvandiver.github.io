+++
title = "cdf: cd by name from cwd"
date = 2026-10-02
description = "Jump to a nested directory by name with fd, without tabbing through the full path."
taxonomies = { tags = ["Bash", "Linux"] }
+++

Add to `~/.bashrc`, then `source ~/.bashrc`. Requires [fd](https://github.com/sharkdp/fd).

```bash
# cdf <pattern>: cd to a directory below cwd matching <pattern>; menu if several match
cdf() {
	# Display help text if no path arg is provided.
    [ -z "$1" ] && { echo "usage: cdf <pattern>"; return 1; }
    # Define the local dirs variable.
    local dirs
    # Create the dirs array variable of the filtered results using the fd command.
    mapfile -t dirs < <(fd --type d --hidden --exclude .git -- "$1" | awk '{ print length, $0 }' | sort -n | cut -d' ' -f2-)
    # Match the cases of the length of the array.
    case ${#dirs[@]} in
    	# If there are 0 matches, then tell the user there is no match.
        0) echo "no match: $1"; return 1 ;;
        # If there is exactly 1 match, the automatically cd to it.
        1) builtin cd -- "${dirs[0]}" ;;
        # If there are 2 or more matches, then
        #  provide a menu of matches to select.
        *) select d in "${dirs[@]}"; do [ -n "$d" ] && builtin cd -- "$d"; break; done ;;
    esac
}
```

- `cdf install` jumps straight there when exactly one directory matches.
- Several matches show a numbered menu, shortest paths first.
- Matching follows `fd`: smart case, regex patterns, and `.gitignore` is respected.
- `builtin cd` bypasses any `cd` alias (such as `alias cd='z'` for zoxide).
